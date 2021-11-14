using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace L05
{
internal class Program
{    private static CloudTable studentsTable_;

    private static CloudTable raportTable_;

// connectionString is the access key to azure cloud storage
    private static readonly string connectionString = Environment.GetEnvironmentVariable("connectionString");

    internal static async Task Main()
    {
        await InitializeRaportTable();
        OpenStudentTable();

        List<StudentEntity> students = await GetAllStudents();

        var mapStud = new Dictionary<string, int>();
        int cntGeneral = 0;
        foreach (StudentEntity s in students)
        {
            if (mapStud.ContainsKey(s.PartitionKey))
                mapStud[s.PartitionKey]++;
            else
                mapStud[s.PartitionKey] = 1;

            cntGeneral++;
        }

        Console.Write(DateTime.Now.ToString("HH:mm:ss") + ": ");
        foreach (KeyValuePair<string, int> s in mapStud)
        {
            RaportEntity raportEntity = new RaportEntity(s.Key, s.Value);
            await CreateRaport(raportEntity);
            Console.Write(s.Key + "->" + s.Value + ";  ");
        }
        RaportEntity raportEntity2 = new RaportEntity("General", cntGeneral);
        await CreateRaport(raportEntity2);
        Console.WriteLine("General->" + cntGeneral.ToString());
    }

    public static async Task<List<StudentEntity>> GetAllStudents()
    {
        var students = new List<StudentEntity>();

        TableQuery<StudentEntity> query = new TableQuery<StudentEntity>();

        TableContinuationToken token = null;

        do
        {
            TableQuerySegment<StudentEntity> resultSegment = await studentsTable_.ExecuteQuerySegmentedAsync(query, token);
            token = resultSegment.ContinuationToken;

            students.AddRange(resultSegment.Results);
        } while (token != null);

        return students;
    }

    public static async Task CreateRaport(RaportEntity raport)
    {
        var insertOperation = TableOperation.Insert(raport);

        await raportTable_.ExecuteAsync(insertOperation);
    }

    private static void OpenStudentTable()
    {
        studentsTable_ = CloudStorageAccount.Parse(connectionString).CreateCloudTableClient().GetTableReference("Studenti");
    }

    private static async Task InitializeRaportTable()
    {
        raportTable_ = CloudStorageAccount.Parse(connectionString).CreateCloudTableClient().GetTableReference("Rapoarte");
        await raportTable_.CreateIfNotExistsAsync();
    }
}
}