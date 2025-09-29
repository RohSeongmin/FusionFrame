using System.Collections.Generic;
using UnityEngine;
//using Firebase.Firestore;
using System.Threading.Tasks;

// This Code is for Function for Firebase (Firestore).
// It is not needed anymore because this code is for server.

public class FirestoreManager : MonoBehaviour
{
    //public async Task<bool> AddPlayer(string playerName, int level)
    //{
    //    var docRef = FirebaseInit.db.Collection("players").Document(playerName);
    //    var snapshot = await docRef.GetSnapshotAsync();
    //
    //    if (snapshot.Exists)
    //    {
    //        Debug.Log("Already existing Name: " + playerName);
    //        return false; 
    //    }
    //
    //    await docRef.SetAsync(new Dictionary<string, object>
    //    {
    //        { "name", playerName },
    //        { "level", level }
    //    });
    //
    //    Debug.Log("Complete to add player: " + playerName);
    //    return true;
    //}

    //public async Task UpdateLevel(string playerName, int newLevel)
    //{
    //    var docRef = FirebaseInit.db.Collection("players").Document(playerName);
    //    var snapshot = await docRef.GetSnapshotAsync();
    //
    //    if (!snapshot.Exists)
    //    {
    //        Debug.Log("Cannot find Player: " + playerName);
    //        return;
    //    }

    //    await docRef.UpdateAsync("level", newLevel);
    //    Debug.Log("Level Updated: " + playerName + " ¡æ " + newLevel);
    //}

    //public async Task<List<PlayerData>> GetRanking(int topN = 10)
    //{
    //    var query = FirebaseInit.db.Collection("players")
    //        .OrderByDescending("level")
    //        .Limit(topN);
    //
    //    var snapshot = await query.GetSnapshotAsync();
    //
    //    var ranking = new List<PlayerData>();
    //    foreach (var doc in snapshot.Documents)
    //    {
    //        ranking.Add(new PlayerData
    //        {
    //            name = doc.GetValue<string>("name"),
    //            level = doc.GetValue<int>("level")
    //        });
    //    }

    //    Debug.Log("==== Top " + topN + " Ranking ====");
    //    foreach (var player in ranking)
    //    {
    //        Debug.Log(player.name + " : " + player.level);
    //    }

    //    return ranking;
    //}
}

//public class PlayerData
//{
//    public string name;
//    public int level;
//}
