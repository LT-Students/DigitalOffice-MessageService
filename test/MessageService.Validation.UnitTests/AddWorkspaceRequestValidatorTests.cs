using FluentValidation;
using FluentValidation.TestHelper;
using LT.DigitalOffice.MessageService.Models.Dto;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.MessageService.Validation.UnitTests
{
    public class AddWorkspaceRequestValidatorTests
    {
        private IValidator<AddWorkspaceRequest> validator;
        private AddWorkspaceRequest workspace;

        [SetUp]
        public void SetUp()
        {
            validator = new AddWorkspaceRequestValidator();

            workspace = new AddWorkspaceRequest
            {
                Name = "Name",
                Description = "Description"
            };
        }

        [Test]
        public void ShouldThrowValidationExceptionWhenNameIsNull()
        {
            validator.ShouldHaveValidationErrorFor(x => x.Name, string.Empty);
        }

        [Test]
        public void ShouldNotHaveAnyValidationErrorsWhenRequestIsValid()
        {
            validator.TestValidate(workspace).ShouldNotHaveAnyValidationErrors();
        }
    }
}
