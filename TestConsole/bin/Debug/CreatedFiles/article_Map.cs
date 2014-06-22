using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using Helper;
namespace CommonClass
{
    public partial class article : BaseMap
    {
        public override IDictionary<string, object> Serialize()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
            
            dic["Idx"] = Idx;
            dic["Title"] = Title;
            dic["Content"] = Content;
            dic["Icon"] = Icon;
            dic["Click"] = Click;
            dic["Istop"] = Istop;
            dic["Indate"] = Indate;
            dic["Url"] = Url;
            dic["authorid"] = authorid;
            dic["cid"] = cid;
            dic["kwd"] = kwd;
            dic["desc"] = desc;
            dic["SourceTxt"] = SourceTxt;
            dic["SourceUrl"] = SourceUrl;
            dic["AuthorNick"] = AuthorNick;
            dic["guids"] = guids;
            dic["img1"] = img1;
            dic["img2"] = img2;
            dic["img3"] = img3;
            dic["img4"] = img4;
            dic["img5"] = img5;
            dic["img6"] = img6;
            dic["img7"] = img7;
            dic["img8"] = img8;
            dic["img9"] = img9;
            dic["img10"] = img10;
            dic["Tags"] = Tags;
            dic["UName"] = UName;
            dic["ContentMode"] = ContentMode;
            dic["Subject"] = Subject; 
            return dic;
        }
        public override void Deserialise(IDictionary<string, object> dic)
        {
            
            Idx=GetVal<Int32>(dic, "Idx");
            Title=GetVal<String>(dic, "Title");
            Content=GetVal<String>(dic, "Content");
            Icon=GetVal<String>(dic, "Icon");
            Click=GetVal<Int32>(dic, "Click");
            Istop=GetVal<Int32>(dic, "Istop");
            Indate=GetVal<DateTime>(dic, "Indate");
            Url=GetVal<String>(dic, "Url");
            authorid=GetVal<Int32>(dic, "authorid");
            cid=GetVal<Int32>(dic, "cid");
            kwd=GetVal<String>(dic, "kwd");
            desc=GetVal<String>(dic, "desc");
            SourceTxt=GetVal<String>(dic, "SourceTxt");
            SourceUrl=GetVal<String>(dic, "SourceUrl");
            AuthorNick=GetVal<String>(dic, "AuthorNick");
            guids=GetVal<String>(dic, "guids");
            img1=GetVal<String>(dic, "img1");
            img2=GetVal<String>(dic, "img2");
            img3=GetVal<String>(dic, "img3");
            img4=GetVal<String>(dic, "img4");
            img5=GetVal<String>(dic, "img5");
            img6=GetVal<String>(dic, "img6");
            img7=GetVal<String>(dic, "img7");
            img8=GetVal<String>(dic, "img8");
            img9=GetVal<String>(dic, "img9");
            img10=GetVal<String>(dic, "img10");
            Tags=GetVal<String>(dic, "Tags");
            UName=GetVal<String>(dic, "UName");
            ContentMode=GetVal<String>(dic, "ContentMode");
            Subject=GetVal<String>(dic, "Subject"); 
        }
    }
}
