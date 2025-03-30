namespace PoZiomkaDomain.Student.Dtos;

public record StudentEdit(int Id, string Email,
	string FirstName, string LastName,
	string? PhoneNumber, string? IndexNumber,
	bool IsPhoneNumberHidden, bool IsIndexNumberHidden)
{

};