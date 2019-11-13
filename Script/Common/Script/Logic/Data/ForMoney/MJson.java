package com.bytedance.android;
import java.util.ArrayList;;

public class MJson {

	public class JsonData{
		public String key;
		public Object Value;
	}
	
	ArrayList<JsonData> m_arrayList = new ArrayList<JsonData>();
	
	public Object GetValue(String key)
	{
		if(key == null || key.isEmpty())
			return null;
		for(int i=0;i < m_arrayList.size();i++)
		{
			JsonData json = m_arrayList.get(i);
			if(key.equals(json.key))
			{
				return json.Value;
			}
		}
		return null;
	}
	
	public void AddJson(String key,Object Value)
	{
		if(key == null || key.isEmpty())
			return;
		JsonData json = new JsonData();
		json.key = key;
		json.Value = Value;
		m_arrayList.add(json);
	}
	
	public void DelJson(String key)
	{
		if(key == null || key.isEmpty())
			return;
		for(int i=0;i < m_arrayList.size();i++)
		{
			JsonData json = m_arrayList.get(i);
			if(key.equals(json.key))
			{
				m_arrayList.remove(i);
				return;
			}
		}
	}
	
	public void CreateFromJson(String jsonStr)
	{
		jsonStr = jsonStr.replace("{", "");
		jsonStr = jsonStr.replace("}", "");
		CreateFromString(jsonStr);
	}
	
	public void CreateFromString(String jsonStr)
	{
		String[] strList = jsonStr.split(",");
		for(int i=0;i<strList.length;i++)
		{
			String item = strList[i];
			if(item.contains(":")==false)
				continue;
			String[] itemList = item.split(":");
			if(itemList.length < 2)
				continue;
			String key = itemList[0];
			String value = itemList[1];
			value = value.replace("\"", "");
			key = key.replace("\"", "");
			AddJson(key,value);
		}
	}
	
	public String ToJsonString()
	{
		String jsonStr = "{";
		int len = m_arrayList.size() - 1;
		for(int i=0;i <= len;i++)
		{
			JsonData json = m_arrayList.get(i);
			if(i==len)
			{
				String newStr = String.format("\"%s\":\"%s\"}",json.key,json.Value);
				jsonStr = jsonStr.concat(newStr);
			}
			else
			{
				String newStr = String.format("\"%s\":\"%s\",",json.key,json.Value);
				jsonStr = jsonStr.concat(newStr);	
			}
		}
		return jsonStr;
	}
	
}
