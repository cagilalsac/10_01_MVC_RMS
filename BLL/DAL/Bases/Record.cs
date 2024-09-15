#nullable disable
// For preventing the usage of ? (nullable) for reference types
// such as strings, arrays, classes, interfaces, etc.
// Should only be used with entity and model classes.

namespace BLL.DAL.Bases // namespace is used for grouping the classes according the their similar purposes,
                        // similar to package usage in Java
{
    // Record is an abstract base class for all of the entities, which contains the common properties
    // such as Id, Guid, CreateDate, CreatedBy, UpdateDate, UpdatedBy, IsDeleted, etc.
    public abstract class Record
    {
        // data member and member method usage example from Java:
        // private int id; // a class variable is called as a field in C#

        // public void setId(int id) // a class method is called as a behavior in C#
        // {
        //     this.id = id;
        // }

        // public int getId()
        // {
        //     return id;
        // }



        // Way 1 (Encapsulation in C#):
        //private int _id; // private and protected field names start with "_" in C#

        //public int Id
        //{
        //    get // getter
        //    {
        //        return _id;
        //    }
        //    set // setter
        //    {
        //        _id = value;
        //    }
        //}
        // Way 2: no need to implement encapsulation since it is automatically implemented by properties
        public int Id { get; set; } // this is called a property in C# which contains getters and setters,
                                    // will be primary key in the related database table,
                                    // Id property will be inherited to all child entity classes,
                                    // value assignment is required
                                    // because the type of the property is "int" which is a value type in C#,
                                    // if not assigned during object initialization, 0 will be assigned as default value



        // integer number data types (value types) in C#: int, long, short, byte,
        // boolean data type (value type) in C#: bool,
        // decimal number data types (value types) in C#: decimal, double, float,
        // character data type (value type) in C#: char,
        // reference types in C#: string, arrays, classes, interfaces
    }
}
