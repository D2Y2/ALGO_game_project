#if false
using Mono.Data.Sqlite;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SQLiteManager : MonoBehaviour
{
    private SqliteConnection dbconn;
    private string dbPath;
    
    // 싱글톤 패턴으로 어디서든 접근 가능하게 합니다.
    public static SQLiteManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // 모바일 환경을 고려하여 지속적인 데이터 경로에 파일 생성
        dbPath = Path.Combine(Application.persistentDataPath, "game_data.db");
        
        Debug.Log($"데이터베이스 경로: {dbPath}");
        
        InitializeDatabase();
    }

    private void InitializeDatabase()
    {
        // 데이터베이스 파일이 없으면 새로 생성
        if (!File.Exists(dbPath))
        {
            SqliteConnection.CreateFile(dbPath);
            Debug.Log("데이터베이스 파일 생성 완료!");
        }

        // 데이터베이스 연결
        dbconn = new SqliteConnection($"URI=file:{dbPath};");
        dbconn.Open();

        // 테이블 생성
        CreateTable();
    }

    private void CreateTable()
    {
        string sql = "CREATE TABLE IF NOT EXISTS player_data (" +
                     "player_id INTEGER PRIMARY KEY, " +
                     "player_name TEXT NOT NULL UNIQUE, " +
                     "health INTEGER, " +
                     "speed INTEGER, " +
                     "mentality INTEGER, " +
                     "equipped_weapon TEXT);";
        
        SqliteCommand command = new SqliteCommand(sql, dbconn);
        command.ExecuteNonQuery();
        Debug.Log("player_data 테이블 생성 또는 확인 완료.");
    }

    private void OnApplicationQuit()
    {
        if (dbconn != null && dbconn.State == System.Data.ConnectionState.Open)
        {
            dbconn.Close();
            Debug.Log("데이터베이스 연결 종료.");
        }
    }

    // --- 데이터 저장 함수 ---
    public void SaveGameData(string playerName, int health, int speed, int mentality, string equippedWeapon)
    {
        string sql = "INSERT OR REPLACE INTO player_data (player_name, health, speed, mentality, equipped_weapon) VALUES (@playerName, @health, @speed, @mentality, @equippedWeapon);";
        
        SqliteCommand command = new SqliteCommand(sql, dbconn);
        command.Parameters.AddWithValue("@playerName", playerName);
        command.Parameters.AddWithValue("@health", health);
        command.Parameters.AddWithValue("@speed", speed);
        command.Parameters.AddWithValue("@mentality", mentality);
        command.Parameters.AddWithValue("@equippedWeapon", equippedWeapon);

        command.ExecuteNonQuery();
        Debug.Log("게임 데이터 저장 완료!");
    }

    // --- 데이터 불러오기 함수 ---
    public Dictionary<string, object> LoadGameData(string playerName)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        string sql = "SELECT health, speed, mentality, equipped_weapon FROM player_data WHERE player_name = @playerName;";
        
        SqliteCommand command = new SqliteCommand(sql, dbconn);
        command.Parameters.AddWithValue("@playerName", playerName);

        SqliteDataReader reader = command.ExecuteReader();
        if (reader.Read())
        {
            data["health"] = reader.GetInt32(0);
            data["speed"] = reader.GetInt32(1);
            data["mentality"] = reader.GetInt32(2);
            data["equipped_weapon"] = reader.GetString(3);
            Debug.Log("게임 데이터 불러오기 성공!");
        }
        else
        {
            Debug.LogWarning("플레이어 데이터를 찾을 수 없습니다.");
        }
        reader.Close();
        return data;
    }
}
#endif