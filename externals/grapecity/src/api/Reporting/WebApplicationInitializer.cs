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
 * This file is part of Zongsoft.Externals.GrapeCity library.
 *
 * The Zongsoft.Externals.GrapeCity is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation, either version 3.0 of the License,
 * or (at your option) any later version.
 *
 * The Zongsoft.Externals.GrapeCity is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with the Zongsoft.Externals.GrapeCity library. If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Linq;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

using GrapeCity.ActiveReports;
using GrapeCity.ActiveReports.Aspnetcore.Viewer;

using Zongsoft.Services;
using Zongsoft.Reporting;

namespace Zongsoft.Externals.Grapecity.Reporting.Web
{
	[Service(typeof(IApplicationInitializer<IApplicationBuilder>))]
	public class WebApplicationInitializer : Zongsoft.Services.IApplicationInitializer<IApplicationBuilder>
	{
		#region 初始方法
		public void Initialize(IApplicationBuilder builder)
		{
			builder.UseReporting(settings =>
			{
				settings.UseCompression = true;

				settings.UseCustomStore(name =>
				{
					//var filePath = System.IO.Path.Combine(ApplicationContext.Current.ApplicationPath, "reports", name);
					//var fileInfo = new System.IO.FileInfo(filePath);

					//if(fileInfo.Exists)
					//{
					//	var report = new PageReport(fileInfo);
					//	return report;
					//}

					//return null;

					var providers = builder.ApplicationServices.ResolveAll<IReportLocator>().OrderByDescending(p => p.Priority);

					foreach(var provider in providers)
					{
						var descriptor = provider.GetReport(name);

						if(descriptor != null)
						{
							var report = Report.Open(descriptor);

							if(report != null)
								return report.AsReport<GrapeCity.ActiveReports.PageReportModel.Report>();
						}
					}

					return null;
				});
			});
		}
		#endregion
	}
}
