namespace Jiper.RavenDB.SampleApp.DotNet8.Model;

public class SampleDocument(string id, string name)
{
    public string Id { get; set; } = id;
    public string Name { get; set; } = name;
}