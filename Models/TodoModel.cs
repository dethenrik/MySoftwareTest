namespace SoftwareTest.Models
{
    // Models/Cprog.cs
    public class Cpr
    {
        public int Id { get; set; }
        public string UserId { get; set; }  // This is to link to the User
        public string CprNumber { get; set; }

        // Navigation property for related TodoItems
        public ICollection<TodoItem> TodoItems { get; set; }
    }


    // Models/Todolist.cs
    public class TodoItem
    {
        public int Id { get; set; }
        public string Description { get; set; }

        // Foreign Key to Cpr
        public int CprTableId { get; set; }
        public Cpr CprTable { get; set; }  // Navigation property to Cpr
    }


}
