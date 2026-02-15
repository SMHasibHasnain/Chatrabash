using System;
using System.Data;
using FluentValidation;

namespace Application.Rooms.Validators;

public class Create : BaseRoomsValidator<Rooms.Commands.Create.Command, Commands.Create.Command>
{
    public Create() : base( x => x)
    {
        
    }
}
