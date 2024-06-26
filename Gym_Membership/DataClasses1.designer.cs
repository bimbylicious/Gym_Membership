﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Gym_Membership
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="GymMembership")]
	public partial class DataClasses1DataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    #endregion
		
		public DataClasses1DataContext() : 
				base(global::Gym_Membership.Properties.Settings.Default.GymMembershipConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public DataClasses1DataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DataClasses1DataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DataClasses1DataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DataClasses1DataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<Customer> Customers
		{
			get
			{
				return this.GetTable<Customer>();
			}
		}
		
		public System.Data.Linq.Table<Staff> Staffs
		{
			get
			{
				return this.GetTable<Staff>();
			}
		}
		
		public System.Data.Linq.Table<Transaction> Transactions
		{
			get
			{
				return this.GetTable<Transaction>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Customer")]
	public partial class Customer
	{
		
		private string _Customer_ID;
		
		private string _Customer_Name;
		
		private string _Transaction_ID;
		
		private string _Status;
		
		private string _Staff_ID;
		
		private string _Membership_Length;
		
		public Customer()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Customer_ID", DbType="VarChar(60)")]
		public string Customer_ID
		{
			get
			{
				return this._Customer_ID;
			}
			set
			{
				if ((this._Customer_ID != value))
				{
					this._Customer_ID = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Customer_Name", DbType="VarChar(100)")]
		public string Customer_Name
		{
			get
			{
				return this._Customer_Name;
			}
			set
			{
				if ((this._Customer_Name != value))
				{
					this._Customer_Name = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Transaction_ID", DbType="VarChar(60)")]
		public string Transaction_ID
		{
			get
			{
				return this._Transaction_ID;
			}
			set
			{
				if ((this._Transaction_ID != value))
				{
					this._Transaction_ID = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Status", DbType="VarChar(60)")]
		public string Status
		{
			get
			{
				return this._Status;
			}
			set
			{
				if ((this._Status != value))
				{
					this._Status = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Staff_ID", DbType="VarChar(60)")]
		public string Staff_ID
		{
			get
			{
				return this._Staff_ID;
			}
			set
			{
				if ((this._Staff_ID != value))
				{
					this._Staff_ID = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Membership_Length", DbType="VarChar(60)")]
		public string Membership_Length
		{
			get
			{
				return this._Membership_Length;
			}
			set
			{
				if ((this._Membership_Length != value))
				{
					this._Membership_Length = value;
				}
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Staff")]
	public partial class Staff
	{
		
		private string _Staff_ID;
		
		private string _Staff_Name;
		
		private string _Staff_Username;
		
		private string _Staff_Password;
		
		private string _Staff_Role;
		
		public Staff()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Staff_ID", DbType="VarChar(60)")]
		public string Staff_ID
		{
			get
			{
				return this._Staff_ID;
			}
			set
			{
				if ((this._Staff_ID != value))
				{
					this._Staff_ID = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Staff_Name", DbType="VarChar(60)")]
		public string Staff_Name
		{
			get
			{
				return this._Staff_Name;
			}
			set
			{
				if ((this._Staff_Name != value))
				{
					this._Staff_Name = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Staff_Username", DbType="VarChar(60)")]
		public string Staff_Username
		{
			get
			{
				return this._Staff_Username;
			}
			set
			{
				if ((this._Staff_Username != value))
				{
					this._Staff_Username = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Staff_Password", DbType="VarChar(60)")]
		public string Staff_Password
		{
			get
			{
				return this._Staff_Password;
			}
			set
			{
				if ((this._Staff_Password != value))
				{
					this._Staff_Password = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Staff_Role", DbType="VarChar(60)")]
		public string Staff_Role
		{
			get
			{
				return this._Staff_Role;
			}
			set
			{
				if ((this._Staff_Role != value))
				{
					this._Staff_Role = value;
				}
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Transactions")]
	public partial class Transaction
	{
		
		private string _Transaction_ID;
		
		private string _Membership_Added_Length;
		
		private string _Customer_ID;
		
		private System.Nullable<double> _Total_Price;
		
		private System.Nullable<double> _Total_Change;
		
		public Transaction()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Transaction_ID", DbType="VarChar(60)")]
		public string Transaction_ID
		{
			get
			{
				return this._Transaction_ID;
			}
			set
			{
				if ((this._Transaction_ID != value))
				{
					this._Transaction_ID = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Membership_Added_Length", DbType="VarChar(255)")]
		public string Membership_Added_Length
		{
			get
			{
				return this._Membership_Added_Length;
			}
			set
			{
				if ((this._Membership_Added_Length != value))
				{
					this._Membership_Added_Length = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Customer_ID", DbType="VarChar(60)")]
		public string Customer_ID
		{
			get
			{
				return this._Customer_ID;
			}
			set
			{
				if ((this._Customer_ID != value))
				{
					this._Customer_ID = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Total_Price", DbType="Float")]
		public System.Nullable<double> Total_Price
		{
			get
			{
				return this._Total_Price;
			}
			set
			{
				if ((this._Total_Price != value))
				{
					this._Total_Price = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Total_Change", DbType="Float")]
		public System.Nullable<double> Total_Change
		{
			get
			{
				return this._Total_Change;
			}
			set
			{
				if ((this._Total_Change != value))
				{
					this._Total_Change = value;
				}
			}
		}
	}
}
#pragma warning restore 1591
