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
 * This file is part of Zongsoft.Externals.WeChat library.
 *
 * The Zongsoft.Externals.WeChat is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation, either version 3.0 of the License,
 * or (at your option) any later version.
 *
 * The Zongsoft.Externals.WeChat is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with the Zongsoft.Externals.WeChat library. If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.ComponentModel;

namespace Zongsoft.Externals.Wechat.Paying
{
	/// <summary>
	/// 表示支付状态的枚举。
	/// </summary>
	public enum PaymentStatus
	{
		/// <summary>未支付</summary>
		[Zongsoft.ComponentModel.Alias("NotPay")]
		None,

		/// <summary>支付成功</summary>
		[Zongsoft.ComponentModel.Alias("Success")]
		Succeed,

		/// <summary>转入退款</summary>
		[Zongsoft.ComponentModel.Alias("Refund")]
		Refund,

		/// <summary>已关闭</summary>
		[Zongsoft.ComponentModel.Alias("Closed")]
		Cancelled,

		/// <summary>已撤销（仅付款码支付会返回）</summary>
		[Zongsoft.ComponentModel.Alias("Revoked")]
		Revoked,

		/// <summary>用户支付中（仅付款码支付会返回）</summary>
		[Zongsoft.ComponentModel.Alias("UserPaying")]
		Paying,

		/// <summary>支付失败（仅付款码支付会返回）</summary>
		[Zongsoft.ComponentModel.Alias("PayError")]
		Failed,
	}
}
