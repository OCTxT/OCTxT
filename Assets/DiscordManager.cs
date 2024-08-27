using System;
using UnityEngine;

public class DiscordManager : MonoBehaviour
{
    Discord.Discord discord;
    private float refreshInterval = 15f; // Refresh interval in seconds
    private float timeSinceLastRefresh = 0f;

    void Start()
    {
        discord = new Discord.Discord(1277794993964187729, (ulong)Discord.CreateFlags.NoRequireDiscord);
        ChangeActivity();
    }

    private void OnDisable()
    {
        discord.Dispose();
    }

    void Update()
    {
        discord.RunCallbacks();

        // Check if it's time to refresh the activity
        timeSinceLastRefresh += Time.deltaTime;
        if (timeSinceLastRefresh >= refreshInterval)
        {
            ChangeActivity();
            timeSinceLastRefresh = 0f; // Reset the timer
        }
    }

    public void ChangeActivity()
    {
        var activityManager = discord.GetActivityManager();
        var activity = new Discord.Activity
        {
            Details = "Texting",
            State = "OCTxT v" + Application.version,
            Assets =
            {
                LargeImage = "OCTxT",
            }

        };
            activityManager.UpdateActivity(activity, (res) =>
            {
                if (res == Discord.Result.Ok)
                {
                    Console.WriteLine("Success!");
                }
                else
                {
                    Console.WriteLine("Failed");
                }
            });
        
    }
}
