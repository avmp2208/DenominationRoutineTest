using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RestServerTest.Common;

namespace RestServerTest.Repository.Entities;

public class Customer
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }

    [Required] 
    public string FirstName { get; set; }

    [Required] 
    public string LastName { get; set; }

    [Required] 
    [AgeValidator] 
    public int Age { get; set; }
}