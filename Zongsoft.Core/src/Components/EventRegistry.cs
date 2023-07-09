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
 * Copyright (C) 2010-2023 Zongsoft Studio <http://www.zongsoft.com>
 *
 * This file is part of Zongsoft.Core library.
 *
 * The Zongsoft.Core is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation, either version 3.0 of the License,
 * or (at your option) any later version.
 *
 * The Zongsoft.Core is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with the Zongsoft.Core library. If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;

namespace Zongsoft.Components
{
	[System.Reflection.DefaultMember(nameof(Events))]
	public class EventRegistry : IEnumerable<EventDescriptor>
	{
		#region 构造函数
		public EventRegistry()
		{
			this.Events = new EventDescriptorCollection();
		}
		#endregion

		#region 公共属性
		/// <summary>获取事件描述器集合。</summary>
		public EventDescriptorCollection Events { get; }
		#endregion

		#region 公共方法
		public ValueTask RaiseAsync(string name, object argument, CancellationToken cancellation = default) => this.RaiseAsync(name, argument, null, cancellation);
		public ValueTask RaiseAsync(string name, object argument, IEnumerable<KeyValuePair<string, object>> parameters, CancellationToken cancellation = default)
		{
			if(string.IsNullOrEmpty(name))
				throw new ArgumentNullException(nameof(name));

			if(this.Events.TryGetValue(name, out var descriptor) && descriptor != null)
				return descriptor.HandleAsync(argument, parameters, cancellation);
			else
				return ValueTask.CompletedTask;
		}

		public ValueTask RaiseAsync<T>(string name, T argument, CancellationToken cancellation = default) => this.RaiseAsync(name, argument, null, cancellation);
		public ValueTask RaiseAsync<T>(string name, T argument, IEnumerable<KeyValuePair<string, object>> parameters, CancellationToken cancellation = default)
		{
			if(string.IsNullOrEmpty(name))
				throw new ArgumentNullException(nameof(name));

			if(this.Events.TryGetValue(name, out var descriptor) && descriptor != null)
				return descriptor.HandleAsync(argument, parameters, cancellation);
			else
				return ValueTask.CompletedTask;
		}
		#endregion

		#region 遍历枚举
		public IEnumerator<EventDescriptor> GetEnumerator() => this.Events.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
		#endregion
	}
}