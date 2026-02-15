using System;
using FluentValidation;

namespace Application.Rooms.Validators;

public class BaseRoomsValidator<T, TDto> : AbstractValidator<T> where TDto : Commands.Create.Command
{

    public BaseRoomsValidator(Func<T, TDto> selector)
    {
        RuleFor(x => selector(x).RoomNumber).NotEmpty().WithMessage("Room Number is required!");
        RuleFor(x => selector(x).FloorNo).NotEmpty().WithMessage("Floor Number is required!");
        RuleFor(x => selector(x).SeatAvailable).NotEmpty().WithMessage("Cannot be empty");
        RuleFor(x => selector(x).SeatCapacity).NotNull().GreaterThan(0).WithMessage("Must be more than 0");
    }

}
