//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SaaMedW
{
    using System;
    using System.Collections.Generic;
    
    public partial class PersonalSpecialty
    {
        public int Id { get; set; }
        public int PersonalId { get; set; }
        public int SpecialtyId { get; set; }
    
        public virtual Personal Personal { get; set; }
        public virtual Specialty Specialty { get; set; }
    }
}