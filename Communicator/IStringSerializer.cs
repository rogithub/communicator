namespace Communicator
{
    internal interface IStringSerializer
	{
		string Serialize<T>(T cSharpObj);
	}

	internal interface IStringDeserializer
	{
		T Deserialize<T>(string serializedObj);
	}
}