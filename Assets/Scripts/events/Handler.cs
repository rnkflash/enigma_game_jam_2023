using UnityEngine;

public abstract class Handler<T>: MonoBehaviour where T : Message
{
	public abstract void HandleMessage(T message);

	protected virtual void Awake()
	{
		EventBus<T>.Sub(HandleMessage);
	}

	protected virtual void OnDestroy()
	{
		EventBus<T>.Unsub(HandleMessage);
	}
}
