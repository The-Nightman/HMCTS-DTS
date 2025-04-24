import { JSX } from "react";

/**
 * RegisterPage Component
 * 
 * A component that renders a user registration form with email and password fields.
 * The form includes validation for:
 * - Email field with proper email format
 * - Password field with specific requirements:
 *   - At least 8 characters
 *   - At least 1 capital letter
 *   - At least 1 lowercase letter
 *   - At least 1 number
 *   - At least 1 special character
 * - Password confirmation field
 * 
 * @remarks
 * Uses GOV.UK-style design patterns for styling and accessibility.
 * 
 * @returns {JSX.Element} A registration form with email and password fields
 */
const RegisterPage = (): JSX.Element => {
  // Not yet implemented
  const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    const formData = new FormData(e.currentTarget);
    const email = formData.get("email");
    const password = formData.get("new-password");
    const confirmPassword = formData.get("confirm-new-password");
  };

  return (
    <>
      <h1 className="m-[10px_0_30px_0] text-3xl md:text-5xl font-bold font-[Helvetica]">
        Create an account
      </h1>
      <p className="text-lg mb-[20px]">
        You'll need your password when you sign in online, so make it memorable.
      </p>
      <form onSubmit={(e) => handleSubmit(e)} className="flex flex-col gap-4">
        <div className="flex flex-col sm:w-1/2">
          <label
            className="text-lg md:text-2xl font-bold mb-[8px]"
            htmlFor="email"
          >
            Email
          </label>
          <input
            className="focus:outline-3 focus:outline-[#ffdd00] border-2 focus:shadow-[inset_0_0_0_2px] border-black p-1"
            id="email"
            name="email"
            type="email"
            aria-description="Enter your email address"
            autoComplete="email"
          />
        </div>
        <div className="flex flex-col sm:w-1/2">
          <label
            className="text-lg md:text-2xl font-bold"
            htmlFor="new-password"
          >
            Create password
          </label>
          <div
            className="max-w-[30em] leading-5 text-[#6f777b] mb-[8px]"
            id="new-password-description"
          >
            Must contain at least 8 characters with at least 1 capital letter, 1
            lower case letter and 1 number.
            <br />
            Do not use your username, a common word like 'password' or a
            sequence like '123'.
          </div>
          <input
            className="focus:outline-3 focus:outline-[#ffdd00] border-2 focus:shadow-[inset_0_0_0_2px] border-black p-1"
            id="new-password"
            name="new-password"
            type="password"
            autoComplete="new-password"
            aria-describedby="new-password-description"
            pattern="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$"
          />
        </div>
        <div className="flex flex-col sm:w-1/2">
          <label
            className="text-lg md:text-2xl font-bold mb-[8px]"
            htmlFor="confirm-new-password"
          >
            Re-type your password
          </label>
          <input
            className="focus:outline-3 focus:outline-[#ffdd00] border-2 focus:shadow-[inset_0_0_0_2px] border-black p-1"
            id="confirm-new-password"
            name="confirm-new-password"
            type="password"
            autoComplete="new-password"
            pattern="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$"
          />
        </div>
        <button
          className="w-fit text-center justify-center bg-[#00823b] hover:bg-[#1f6b43] shadow-[0_2px_0_#003418] text-white py-2 px-4 mb-[30px] border-[2px] border-transparent active:border-[#ffdd00] not-active:focus:bg-[#ffdd00] not-active:focus:text-black select-none"
          type="submit"
          draggable="false"
          aria-label="Create an account"
        >
          Create an account
        </button>
      </form>
    </>
  );
};

export default RegisterPage;
