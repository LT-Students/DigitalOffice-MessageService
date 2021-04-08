using FluentValidation;
using FluentValidation.TestHelper;
using LT.DigitalOffice.MessageService.Models.Dto;
using NUnit.Framework;
using System;

namespace LT.DigitalOffice.MessageService.Validation.UnitTests
{
    public class AddWorkspaceRequestValidatorTests
    {
        private IValidator<Workspace> validator;
        private Workspace workspace;

        [SetUp]
        public void SetUp()
        {
            validator = new WorkspaceValidator();

            workspace = new Workspace
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
