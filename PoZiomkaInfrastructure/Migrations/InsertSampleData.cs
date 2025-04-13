using PoZiomkaDomain.Application;
using PoZiomkaInfrastructure.Common;
using System.Reflection;

namespace PoZiomkaInfrastructure.Migrations;

public class InsertSampleDataImpl
{
    public async static void InsertSampleDataMethod(IFileStorage fileStorage)
    {
        List<string> guids =
        [
            "b6f4cfe3-fc21-45fc-95b5-3dd29ea4de4f",
            "c1186e5b-e8e1-4b5b-8ae3-1e0bc4464b97",
            "fb72b7be-0d92-402c-85de-43a8b66becc5",
            "1cb850e0-57f4-4b12-a6a2-239e2b774cb3",
            "2f1a2a6b-5c6c-4877-91e4-d7e3e7f7f3cc",
            "cad3a4c7-baf9-4f35-91c4-4f84e03f1e79",
            "ed53c9d4-0cb7-4aa6-9b4d-9f029c775d71",
            "41caa1aa-9ae8-4c94-8875-9871c08b81b2",
            "04fa14e2-83c4-4f1c-9f62-2ac03e24e195",
            "3cdd89e6-308e-4422-9475-431a87e75a3d",
            "6f44d9d6-0d50-49ff-8d75-5dbb6e80bc7f",
            "2a83c5cf-37c7-4f4d-b0ea-291502ebd435",
            "cf82c9b4-64db-4d0b-8d9d-9ef832e9a574",
            "ae41fadd-93c9-48a7-bdd3-7dd2fd70f845",
            "3f373a47-93dc-4f07-b438-18835d27aeb1"
        ];

        foreach (var guidString in guids)
        {
            Guid guid = Guid.Parse(guidString);

            using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("PoZiomkaInfrastructure.SampleFiles.SampleApplication.pdf")!;
            IFile file = new BlobFile(stream);

            try
            {
                await fileStorage.UploadFile(guid, file);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
