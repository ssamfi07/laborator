using Microsoft.Extensions.DependencyInjection;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api
{
     class Student : TableEntity
    {
        public string Nume { get; set; }
        public string Prenume { get; set; }
        public int An { get; set; }
        public string Nr { get; set; }
        public string Facultate { get; set; }


        public Student(string university, string id)
        {
            this.PartitionKey = university;
            this.RowKey = id;
        }

        public Student(){}
    }
}