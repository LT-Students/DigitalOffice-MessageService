using FluentValidation;
using FluentValidation.TestHelper;
using LT.DigitalOffice.MessageService.Models.Dto.Requests.Workspace;
using LT.DigitalOffice.MessageService.Validation.Validators.Workspace;
using NUnit.Framework;

namespace LT.DigitalOffice.MessageService.Validation.UnitTests
{
  public class CreateWorkspaceRequestValidatorTests
    {
        private IValidator<CreateWorkspaceRequest> _validator;
        private CreateWorkspaceRequest _workspace;

        [SetUp]
        public void SetUp()
        {
            _validator = new CreateWorkspaceRequestValidator();

            _workspace = new CreateWorkspaceRequest
            {
                Name = "Name",
                Description = "Description"
            };
        }

        [Test]
        public void ShouldThrowValidationExceptionWhenNameIsNull()
        {
            _validator.ShouldHaveValidationErrorFor(x => x.Name, string.Empty);
        }

        [Test]
        public void ShouldNotHaveAnyValidationErrorsWhenRequestIsValid()
        {
            _validator.TestValidate(_workspace).ShouldNotHaveAnyValidationErrors();
        }
    }
}
