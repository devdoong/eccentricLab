using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

namespace Data
{
	public class DropData
	{
		[XmlAttribute]
		public string name;
		[XmlAttribute]
		public string type;
		[XmlAttribute]
		public int exp;
	}


}