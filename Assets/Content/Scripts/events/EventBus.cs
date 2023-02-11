using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventBus<T> where T : Message
{
	public delegate void MessageHandlerDelegate(T message);
	private static MessageHandlerDelegate Handlers = null;
	private static List<MessageHandlerDelegate> MessageHandlers;

	static EventBus()
	{
		MessageHandlers = new List<MessageHandlerDelegate>();
	}

	public static void Clear()
	{
		MessageHandlers.Clear();
	}

	public static void Pub(T message)
	{
		Handlers?.Invoke(message);
	}

	public static void Sub(MessageHandlerDelegate @delegate)
	{
		if (@delegate != null)
		{
			Handlers += @delegate;
		}
	}
	public static void Unsub(MessageHandlerDelegate @delegate)
	{
		if (@delegate != null)
		{
			Handlers -= @delegate;
		}
	}
}
