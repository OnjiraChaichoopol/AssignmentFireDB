using System;
using Google.Cloud.Firestore;
using Maui_FireDB.Models;

namespace Maui_FireDB.Services;

public class FireDBService
{
    private FirestoreDb db;
    public string StatusMessage;

    public FireDBService()
    {
        this.SetupMaui_FireDB();
    }
    private async Task SetupMaui_FireDB()
    {
        if (db == null)
        {
            var stream = await FileSystem.OpenAppPackageFileAsync("firedb-f2914-firebase-adminsdk-84yel-feabf239ce.json");
            var reader = new StreamReader(stream);
            var contents = reader.ReadToEnd();
            db = new FirestoreDbBuilder
            {
                ProjectId = "firedb-f2914",

                JsonCredentials = contents
            }.Build();
        }
    }
    public async Task<List<StudentModel>> GetAllStudent()
    {
        try
        {
            await SetupMaui_FireDB();
            var data = await db.Collection("Students").GetSnapshotAsync();
            var students = data.Documents.Select(doc =>
            {
                var student = new StudentModel();
                student.Id = doc.Id;
                student.Id = doc.GetValue<string>("Id");
                student.Code = doc.GetValue<string>("Code");
                student.Name = doc.GetValue<string>("Name");
                return student;
            }).ToList();
            return students;
        }
        catch (Exception ex)
        {

            StatusMessage = $"Error: {ex.Message}";
        }
        return null;
    }
    public async Task InsertStudent(StudentModel student)
    {
        try
        {
            await SetupMaui_FireDB();
            var studentData = new Dictionary<string, object>
            {
                { "Id", student.Id},
                { "Code", student.Code },
                { "Name", student.Name }
                // Add more fields as needed
            };

            await db.Collection("Students").AddAsync(studentData);
        }
        catch (Exception ex)
        {

            StatusMessage = $"Error: {ex.Message}";
        }
    }
    public async Task UpdateStudent(StudentModel student)
    {
        try
        {
            await SetupMaui_FireDB();

            // Manually create a dictionary for the updated data
            var studentData = new Dictionary<string, object>
            {
                { "Id", student.Id},
                { "Code", student.Code },
                { "Name", student.Name }
                // Add more fields as needed
            };

            // Reference the document by its Id and update it
            var docRef = db.Collection("Students").Document(student.Id);
            await docRef.SetAsync(studentData, SetOptions.Overwrite);

            StatusMessage = "Sample successfully updated!";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error: {ex.Message}";
        }
    }
     public async Task DeleteStudent(string id)
    {
        try
        {
            await SetupMaui_FireDB();

            // Reference the document by its Id and delete it
            var docRef = db.Collection("Students").Document(id);
            await docRef.DeleteAsync();

            StatusMessage = "Student successfully deleted!";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error: {ex.Message}";
        }
    }
}
