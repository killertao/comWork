//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Cooshu.Spider.Model
{
    using Cooshu.EntityStorage;
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;
    
    
    /// <summary>
    /// 
    /// </summary>
    public partial class SourceArticle : DbEntity<SpiderContext, SourceArticle>
    {
    	
    	/// <summary>
    	/// 编号
    	/// </summary>
    	[DisplayName("编号")]
        public int Id 
    	{ 
    		get
    		{
    			return _Id;
    		}
    		set
    		{
    			AddChangedProperty("Id");
    			_Id = value;
    		} 
    	}
    	private int _Id;
    	
    	/// <summary>
    	/// 标题
    	/// </summary>
    	[DisplayName("标题")]
        public string Title 
    	{ 
    		get
    		{
    			return _Title;
    		}
    		set
    		{
    			AddChangedProperty("Title");
    			_Title = value;
    		} 
    	}
    	private string _Title;
    	
    	/// <summary>
    	/// 原文
    	/// </summary>
    	[DisplayName("原文")]
        public string RawText 
    	{ 
    		get
    		{
    			return _RawText;
    		}
    		set
    		{
    			AddChangedProperty("RawText");
    			_RawText = value;
    		} 
    	}
    	private string _RawText;
    	
    	/// <summary>
    	/// 文书号
    	/// </summary>
    	[DisplayName("文书号")]
        public string Number 
    	{ 
    		get
    		{
    			return _Number;
    		}
    		set
    		{
    			AddChangedProperty("Number");
    			_Number = value;
    		} 
    	}
    	private string _Number;
    	
    	/// <summary>
    	/// 发布日期
    	/// </summary>
    	[DisplayName("发布日期")]
        public Nullable<System.DateTime> PublishDate 
    	{ 
    		get
    		{
    			return _PublishDate;
    		}
    		set
    		{
    			AddChangedProperty("PublishDate");
    			_PublishDate = value;
    		} 
    	}
    	private Nullable<System.DateTime> _PublishDate;
    	
    	/// <summary>
    	/// 发院
    	/// </summary>
    	[DisplayName("发院")]
        public string Court 
    	{ 
    		get
    		{
    			return _Court;
    		}
    		set
    		{
    			AddChangedProperty("Court");
    			_Court = value;
    		} 
    	}
    	private string _Court;
    	
    	/// <summary>
    	/// 类型
    	/// </summary>
    	[DisplayName("类型")]
        public string Type 
    	{ 
    		get
    		{
    			return _Type;
    		}
    		set
    		{
    			AddChangedProperty("Type");
    			_Type = value;
    		} 
    	}
    	private string _Type;
    	
    	/// <summary>
    	/// 子类型
    	/// </summary>
    	[DisplayName("子类型")]
        public string SubType 
    	{ 
    		get
    		{
    			return _SubType;
    		}
    		set
    		{
    			AddChangedProperty("SubType");
    			_SubType = value;
    		} 
    	}
    	private string _SubType;
    	
    	/// <summary>
    	/// 文章地址
    	/// </summary>
    	[DisplayName("文章地址")]
        public string Url 
    	{ 
    		get
    		{
    			return _Url;
    		}
    		set
    		{
    			AddChangedProperty("Url");
    			_Url = value;
    		} 
    	}
    	private string _Url;
    	
    	/// <summary>
    	/// 发布机构
    	/// </summary>
    	[DisplayName("发布机构")]
        public string PublishInstitution 
    	{ 
    		get
    		{
    			return _PublishInstitution;
    		}
    		set
    		{
    			AddChangedProperty("PublishInstitution");
    			_PublishInstitution = value;
    		} 
    	}
    	private string _PublishInstitution;
    	
    	/// <summary>
    	/// 执行时间
    	/// </summary>
    	[DisplayName("执行时间")]
        public Nullable<System.DateTime> ExecuteDate 
    	{ 
    		get
    		{
    			return _ExecuteDate;
    		}
    		set
    		{
    			AddChangedProperty("ExecuteDate");
    			_ExecuteDate = value;
    		} 
    	}
    	private Nullable<System.DateTime> _ExecuteDate;
    	
    	/// <summary>
    	/// 状态
    	/// </summary>
    	[DisplayName("状态")]
        public string State 
    	{ 
    		get
    		{
    			return _State;
    		}
    		set
    		{
    			AddChangedProperty("State");
    			_State = value;
    		} 
    	}
    	private string _State;
    	
    	/// <summary>
    	/// 原始数据
    	/// </summary>
    	[DisplayName("原始数据")]
        public string RawHtml 
    	{ 
    		get
    		{
    			return _RawHtml;
    		}
    		set
    		{
    			AddChangedProperty("RawHtml");
    			_RawHtml = value;
    		} 
    	}
    	private string _RawHtml;
    	
    	/// <summary>
    	/// 效力级别
    	/// </summary>
    	[DisplayName("效力级别")]
        public string PotencyLevel 
    	{ 
    		get
    		{
    			return _PotencyLevel;
    		}
    		set
    		{
    			AddChangedProperty("PotencyLevel");
    			_PotencyLevel = value;
    		} 
    	}
    	private string _PotencyLevel;
    	
    	/// <summary>
    	/// 
    	/// </summary>
    	[DisplayName("")]
        public Nullable<bool> Succee 
    	{ 
    		get
    		{
    			return _Succee;
    		}
    		set
    		{
    			AddChangedProperty("Succee");
    			_Succee = value;
    		} 
    	}
    	private Nullable<bool> _Succee;
    	
    	/// <summary>
    	/// 
    	/// </summary>
    	[DisplayName("")]
        public string ExtendProperty 
    	{ 
    		get
    		{
    			return _ExtendProperty;
    		}
    		set
    		{
    			AddChangedProperty("ExtendProperty");
    			_ExtendProperty = value;
    		} 
    	}
    	private string _ExtendProperty;
    	
    	/// <summary>
    	/// 
    	/// </summary>
    	[DisplayName("")]
        public Nullable<int> Gid 
    	{ 
    		get
    		{
    			return _Gid;
    		}
    		set
    		{
    			AddChangedProperty("Gid");
    			_Gid = value;
    		} 
    	}
    	private Nullable<int> _Gid;
    	
    	/// <summary>
    	/// 
    	/// </summary>
    	[DisplayName("")]
        public Nullable<int> PotencyLevelType 
    	{ 
    		get
    		{
    			return _PotencyLevelType;
    		}
    		set
    		{
    			AddChangedProperty("PotencyLevelType");
    			_PotencyLevelType = value;
    		} 
    	}
    	private Nullable<int> _PotencyLevelType;
    	
    	/// <summary>
    	/// 
    	/// </summary>
    	[DisplayName("")]
        public string Append 
    	{ 
    		get
    		{
    			return _Append;
    		}
    		set
    		{
    			AddChangedProperty("Append");
    			_Append = value;
    		} 
    	}
    	private string _Append;
    	
    	/// <summary>
    	/// 
    	/// </summary>
    	[DisplayName("")]
        public string Attachment 
    	{ 
    		get
    		{
    			return _Attachment;
    		}
    		set
    		{
    			AddChangedProperty("Attachment");
    			_Attachment = value;
    		} 
    	}
    	private string _Attachment;
    	
    	/// <summary>
    	/// 采集时间
    	/// </summary>
    	[DisplayName("采集时间")]
        public Nullable<System.DateTime> CreateDate 
    	{ 
    		get
    		{
    			return _CreateDate;
    		}
    		set
    		{
    			AddChangedProperty("CreateDate");
    			_CreateDate = value;
    		} 
    	}
    	private Nullable<System.DateTime> _CreateDate;
    	
    	/// <summary>
    	/// 
    	/// </summary>
    	[DisplayName("")]
        public string Guid 
    	{ 
    		get
    		{
    			return _Guid;
    		}
    		set
    		{
    			AddChangedProperty("Guid");
    			_Guid = value;
    		} 
    	}
    	private string _Guid;
    }
}