using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Message {

	public String Title;

	[TextArea(3, 10)]
	public List<String> messages;
}
