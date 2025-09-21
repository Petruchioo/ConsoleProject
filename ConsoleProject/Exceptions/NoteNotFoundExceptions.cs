using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleProject.Exceptions
{
    internal class NoteNotFoundExceptions : Exception
    {
        public NoteNotFoundExceptions(int id) : base($"Note ID = {id} is not found") { }
    }
}


/*
 * Не понял что ты хотел показать этой конструкцией
 * тк. ArgumentNotFoundException нет в пространстве имен
 * 
 * Скорее всего имелось ввиду:

```csharp
namespace ConsoleProject.Exceptions
{
    internal class NoteNotFoundException : ArgumentNotFoundException
    {
        public NoteNotFoundException(int noteId) : base($"Note with ID = {id} is not found") { }
    }
}
```
*/
