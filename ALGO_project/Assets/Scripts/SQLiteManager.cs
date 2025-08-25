using Mono.Data.Sqlite;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class SQLiteManager : MonoBehaviour
{
    private SqliteConnection dbconn;
    private string dbPath;
    
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
        dbPath = Path.Combine(Application.persistentDataPath, "game_data.db");
        Debug.Log($"[SQLiteManager] DB 경로: {dbPath}");
        
        InitializeDatabase();
        TestConnection();   // ✅ 연결 테스트 바로 실행
    }

    private void InitializeDatabase()
    {
        if (!File.Exists(dbPath))
        {
            using (File.Create(dbPath)) { }
            Debug.Log("[SQLiteManager] 데이터베이스 파일 새로 생성 완료!");
        }

        dbconn = new SqliteConnection($"URI=file:{dbPath};");
        dbconn.Open();
        Debug.Log("[SQLiteManager] DB 연결 성공!");

        CreateTable();
    }

    private void CreateTable()
    {
        string sql = @"CREATE TABLE IF NOT EXISTS player_data (
                        player_id INTEGER PRIMARY KEY, 
                        player_name TEXT NOT NULL UNIQUE, 
                        health INTEGER, 
                        speed INTEGER, 
                        mentality INTEGER, 
                        equipped_weapon TEXT);";
        
        using (var command = new SqliteCommand(sql, dbconn))
        {
            command.ExecuteNonQuery();
        }
        Debug.Log("[SQLiteManager] player_data 테이블 생성 또는 확인 완료.");
    }

    // ✅ 여기 추가: 연결 테스트 함수
    private void TestConnection()
    {
        try
        {
            using (var cmd = new SqliteCommand("SELECT name FROM sqlite_master WHERE type='table';", dbconn))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Debug.Log($"[SQLiteManager] 현재 DB 테이블: {reader.GetString(0)}");
                    }
                }
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"[SQLiteManager] 연결 테스트 실패: {ex.Message}");
        }
    }

    private void OnApplicationQuit()
    {
        if (dbconn != null && dbconn.State == System.Data.ConnectionState.Open)
        {
            dbconn.Close();
            Debug.Log("[SQLiteManager] 데이터베이스 연결 종료.");
        }
    }

    // 기존 SaveGameData, LoadGameData는 그대로 두면 됨
}
