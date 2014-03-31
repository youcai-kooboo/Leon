using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Query;
using Kooboo.CMS.Toolkit;  

namespace Leon.Modules.Management.Models
{
	public class MobileLanguage : TextContent
	{
		public MobileLanguage()
			: base(new TextContent())
		{ }

		public MobileLanguage(TextContent textContent)
			: base(textContent)
		{ }

		public class FieldNames
		{
			public const string Site = "Site";
			public const string LanguageSite = "LanguageSite";
			public const string LanguageName = "LanguageName";
			public const string LanguageCountryCode = "LanguageCountryCode";
			public const string LanguageFlag = "LanguageFlag";
		}
			
		public string Site
		{
			get
			{
				return this.GetString(FieldNames.Site);
			}
			set
			{
				this[FieldNames.Site] = value;
			}
		}
			
		public string LanguageSite
		{
			get
			{
				return this.GetString(FieldNames.LanguageSite);
			}
			set
			{
				this[FieldNames.LanguageSite] = value;
			}
		}
			
		public string LanguageName
		{
			get
			{
				return this.GetString(FieldNames.LanguageName);
			}
			set
			{
				this[FieldNames.LanguageName] = value;
			}
		}
			
		public string LanguageCountryCode
		{
			get
			{
				return this.GetString(FieldNames.LanguageCountryCode);
			}
			set
			{
				this[FieldNames.LanguageCountryCode] = value;
			}
		}
			
		public string LanguageFlag
		{
			get
			{
				return this.GetString(FieldNames.LanguageFlag);
			}
			set
			{
				this[FieldNames.LanguageFlag] = value;
			}
		}
	}
}
