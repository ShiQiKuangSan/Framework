﻿/*
 *   _____                                ______
 *  /_   /  ____  ____  ____  _________  / __/ /_
 *    / /  / __ \/ __ \/ __ \/ ___/ __ \/ /_/ __/
 *   / /__/ /_/ / / / / /_/ /\_ \/ /_/ / __/ /_
 *  /____/\____/_/ /_/\__  /____/\____/_/  \__/
 *                   /____/
 *
 * Authors:
 *   钟峰(Popeye Zhong) <zongsoft@gmail.com>
 *
 * Copyright (C) 2010-2020 Zongsoft Studio <http://www.zongsoft.com>
 *
 * This file is part of Zongsoft.Data library.
 *
 * The Zongsoft.Data is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation, either version 3.0 of the License,
 * or (at your option) any later version.
 *
 * The Zongsoft.Data is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with the Zongsoft.Data library. If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;

namespace Zongsoft.Data.Common.Expressions
{
	public class UpsertStatementVisitor : StatementVisitorBase<UpsertStatement>
	{
		#region 构造函数
		protected UpsertStatementVisitor()
		{
		}
		#endregion

		#region 重写方法
		protected override void OnVisit(IExpressionVisitor visitor, UpsertStatement statement)
		{
			if(statement.Returning != null && statement.Returning.Table != null)
				visitor.Visit(statement.Returning.Table);

			const string SOURCE_ALIAS = "SRC";

			if(statement.Fields == null || statement.Fields.Count == 0)
				throw new DataException("Missing required fields in the upsert statment.");

			visitor.Output.Append("MERGE INTO ");
			visitor.Visit(statement.Table);
			visitor.Output.AppendLine(" USING (SELECT ");

			for(int i = 0; i < statement.Values.Count; i++)
			{
				if(i > 0)
					visitor.Output.Append(",");

				visitor.Visit(statement.Values[i]);
			}

			visitor.Output.AppendLine(") AS " + SOURCE_ALIAS + " (");

			for(int i = 0; i < statement.Fields.Count; i++)
			{
				if(i > 0)
					visitor.Output.Append(",");

				visitor.Output.Append(statement.Fields[i].Name);
			}

			visitor.Output.AppendLine(") ON");

			for(int i = 0; i < statement.Entity.Key.Length; i++)
			{
				var field = Metadata.DataEntityPropertyExtension.GetFieldName(statement.Entity.Key[i], out _);

				if(i > 0)
					visitor.Output.Append(" AND ");

				if(string.IsNullOrEmpty(statement.Table.Alias))
					visitor.Output.Append($"{field}={SOURCE_ALIAS}.{field}");
				else
					visitor.Output.Append($"{statement.Table.Alias}.{field}={SOURCE_ALIAS}.{field}");
			}

			if(statement.Updation.Count > 0)
			{
				visitor.Output.AppendLine();
				visitor.Output.Append("WHEN MATCHED");

				if(statement.Where != null)
				{
					visitor.Output.Append(" AND ");
					visitor.Visit(statement.Where);
				}

				visitor.Output.AppendLine(" THEN");
				visitor.Output.Append("\tUPDATE SET ");

				int index = 0;

				foreach(var item in statement.Updation)
				{
					if(index++ > 0)
						visitor.Output.Append(",");

					visitor.Visit(item.Field);
					visitor.Output.Append("=");
					visitor.Visit(item.Value);
				}
			}

			visitor.Output.AppendLine();
			visitor.Output.AppendLine("WHEN NOT MATCHED THEN");
			visitor.Output.Append("\tINSERT (");

			for(int i = 0; i < statement.Fields.Count; i++)
			{
				if(i > 0)
					visitor.Output.Append(",");

				visitor.Output.Append(visitor.Dialect.GetIdentifier(statement.Fields[i]));
			}

			visitor.Output.Append(") VALUES (");

			for(int i = 0; i < statement.Fields.Count; i++)
			{
				if(i > 0)
					visitor.Output.Append(",");

				visitor.Output.Append(SOURCE_ALIAS + "." + statement.Fields[i].Name);
			}

			visitor.Output.Append(")");

			//输出返回子句
			this.VisitReturning(visitor, statement.Returning);

			visitor.Output.AppendLine(";");
		}
		#endregion

		#region 私有方法
		private void VisitReturning(IExpressionVisitor visitor, ReturningClause returning)
		{
			if(returning == null)
				return;

			visitor.Output.AppendLine();
			visitor.Output.Append("RETURNING ");

			if(returning.Members == null || returning.Members.Count == 0)
				visitor.Output.Append("*");
			else
			{
				int index = 0;

				foreach(var member in returning.Members)
				{
					if(index++ > 0)
						visitor.Output.Append(",");

					visitor.Visit(member.Field);
				}
			}

			if(returning.Table != null)
			{
				visitor.Output.Append(" INTO ");
				visitor.Output.Append(visitor.Dialect.GetIdentifier(returning.Table.Identifier()));
			}
		}
		#endregion
	}
}
