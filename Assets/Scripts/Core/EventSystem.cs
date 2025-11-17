using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Centralized event system for managing game-wide events.
/// Provides a decoupled way for different systems to communicate.
/// </summary>
public static class EventSystem
{
    private static readonly Dictionary<Type, Delegate> eventDictionary = new Dictionary<Type, Delegate>();

    /// <summary>
    /// Subscribe to an event of type T.
    /// </summary>
    /// <typeparam name="T">The event type to subscribe to</typeparam>
    /// <param name="listener">The callback to invoke when the event is raised</param>
    public static void Subscribe<T>(Action<T> listener) where T : struct
    {
        Type eventType = typeof(T);

        if (eventDictionary.TryGetValue(eventType, out Delegate existingDelegate))
        {
            eventDictionary[eventType] = Delegate.Combine(existingDelegate, listener);
        }
        else
        {
            eventDictionary[eventType] = listener;
        }
    }

    /// <summary>
    /// Subscribe to an event with no parameters.
    /// </summary>
    /// <typeparam name="T">The event type to subscribe to</typeparam>
    /// <param name="listener">The callback to invoke when the event is raised</param>
    public static void Subscribe<T>(Action listener) where T : struct
    {
        Type eventType = typeof(T);

        if (eventDictionary.TryGetValue(eventType, out Delegate existingDelegate))
        {
            eventDictionary[eventType] = Delegate.Combine(existingDelegate, listener);
        }
        else
        {
            eventDictionary[eventType] = listener;
        }
    }

    /// <summary>
    /// Unsubscribe from an event of type T.
    /// </summary>
    /// <typeparam name="T">The event type to unsubscribe from</typeparam>
    /// <param name="listener">The callback to remove</param>
    public static void Unsubscribe<T>(Action<T> listener) where T : struct
    {
        Type eventType = typeof(T);

        if (eventDictionary.TryGetValue(eventType, out Delegate existingDelegate))
        {
            Delegate newDelegate = Delegate.Remove(existingDelegate, listener);
            if (newDelegate == null)
            {
                eventDictionary.Remove(eventType);
            }
            else
            {
                eventDictionary[eventType] = newDelegate;
            }
        }
    }

    /// <summary>
    /// Unsubscribe from an event with no parameters.
    /// </summary>
    /// <typeparam name="T">The event type to unsubscribe from</typeparam>
    /// <param name="listener">The callback to remove</param>
    public static void Unsubscribe<T>(Action listener) where T : struct
    {
        Type eventType = typeof(T);

        if (eventDictionary.TryGetValue(eventType, out Delegate existingDelegate))
        {
            Delegate newDelegate = Delegate.Remove(existingDelegate, listener);
            if (newDelegate == null)
            {
                eventDictionary.Remove(eventType);
            }
            else
            {
                eventDictionary[eventType] = newDelegate;
            }
        }
    }

    /// <summary>
    /// Raise an event of type T with data.
    /// </summary>
    /// <typeparam name="T">The event type to raise</typeparam>
    /// <param name="eventData">The data to pass to subscribers</param>
    public static void Raise<T>(T eventData) where T : struct
    {
        Type eventType = typeof(T);

        if (eventDictionary.TryGetValue(eventType, out Delegate existingDelegate))
        {
            if (existingDelegate is Action<T> callback)
            {
                callback.Invoke(eventData);
            }
        }
    }

    /// <summary>
    /// Raise an event of type T with no data.
    /// </summary>
    /// <typeparam name="T">The event type to raise</typeparam>
    public static void Raise<T>() where T : struct
    {
        Type eventType = typeof(T);

        if (eventDictionary.TryGetValue(eventType, out Delegate existingDelegate))
        {
            if (existingDelegate is Action callback)
            {
                callback.Invoke();
            }
        }
    }

    /// <summary>
    /// Clear all event subscriptions. Useful for cleanup between scenes.
    /// </summary>
    public static void Clear()
    {
        eventDictionary.Clear();
    }

    /// <summary>
    /// Clear subscriptions for a specific event type.
    /// </summary>
    /// <typeparam name="T">The event type to clear</typeparam>
    public static void Clear<T>() where T : struct
    {
        Type eventType = typeof(T);
        if (eventDictionary.ContainsKey(eventType))
        {
            eventDictionary.Remove(eventType);
        }
    }
}
