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
        
        // 데이터베이스가 없으면 생성하고, 있으면 연결합니다.
        InitializeDatabase();
    }
    
    // 게임 종료 시 자동으로 데이터를 저장하고 연결을 닫습니다.
    private void OnApplicationQuit()
    {
        SaveGameData("MyPlayer"); 
        
        if (dbconn != null && dbconn.State == System.Data.ConnectionState.Open)
        {
            dbconn.Close();
            Debug.Log("데이터베이스 연결 종료.");
        }
    }

    // 초기화 및 데이터 로드 절차를 모두 포함하는 함수
    private void InitializeDatabase()
    {
        if (!File.Exists(dbPath))
        {
            SqliteConnection.CreateFile(dbPath);
            Debug.Log("새로운 데이터베이스 파일 생성 완료!");
        }

        dbconn = new SqliteConnection($"URI=file:{dbPath};");
        dbconn.Open();

        CreateTable();
        LoadGameData("MyPlayer");
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
    
    // 게임 데이터 저장
    public void SaveGameData(string playerName)
    {
        // TODO: 여기에 저장할 현재 게임 데이터를 가져오는 코드를 추가하세요.
        // 이 부분은 게임 내 실제 플레이어 오브젝트의 데이터를 가져오도록 수정해야 합니다.
        int currentHealth = 100; // 임시 값
        int currentSpeed = 5; // 임시 값
        int currentMentality = 0; // 임시 값
        string currentWeapon = "권총"; // 임시 값

        string sql = "INSERT OR REPLACE INTO player_data (player_name, health, speed, mentality, equipped_weapon) VALUES (@playerName, @health, @speed, @mentality, @equippedWeapon);";
        
        SqliteCommand command = new SqliteCommand(sql, dbconn);
        command.Parameters.AddWithValue("@playerName", playerName);
        command.Parameters.AddWithValue("@health", currentHealth);
        command.Parameters.AddWithValue("@speed", currentSpeed);
        command.Parameters.AddWithValue("@mentality", currentMentality);
        command.Parameters.AddWithValue("@equippedWeapon", currentWeapon);

        command.ExecuteNonQuery();
        Debug.Log($"플레이어 {playerName}의 데이터가 저장되었습니다.");
    }

    // 게임 데이터 로드
    public void LoadGameData(string playerName)
    {
        string sql = "SELECT health, speed, mentality, equipped_weapon FROM player_data WHERE player_name = @playerName;";
        
        SqliteCommand command = new SqliteCommand(sql, dbconn);
        command.Parameters.AddWithValue("@playerName", playerName);

        SqliteDataReader reader = command.ExecuteReader();
        if (reader.Read())
        {
            Debug.Log("저장된 게임 데이터를 불러옵니다.");
            int loadedHealth = reader.GetInt32(0);
            int loadedSpeed = reader.GetInt32(1);
            int loadedMentality = reader.GetInt32(2);
            string loadedWeapon = reader.GetString(3);
            
            // TODO: 불러온 데이터를 게임에 적용하는 코드를 추가하세요.
            Debug.Log($"불러온 데이터: 체력={loadedHealth}, 속도={loadedSpeed}, 정신력={loadedMentality}, 무기={loadedWeapon}");
        }
        else
        {
            Debug.Log($"플레이어 {playerName}의 데이터가 없습니다. 새로운 게임을 시작합니다.");
            // TODO: 새 게임 시작 시 필요한 초기화 로직을 추가하세요.
        }
        reader.Close();
    }
}
