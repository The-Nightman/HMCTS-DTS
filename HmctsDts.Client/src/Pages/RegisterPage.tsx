import React, { JSX, useEffect, useState } from "react";
import { RegisterData } from "../types/registerData";
import { postRegisterUser } from "../api/postRegisterUser";
import { AxiosError } from "axios";
import ErrorSummary from "../Components/UX/ErrorSummary";
import { useNavigate } from "react-router";

interface SuccessState {
  state: "success" | "error" | null;
  content: React.ReactNode;
}

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
  const [successState, setSuccessState] = useState<SuccessState>({
    state: null,
    content: <></>,
  });
  const navigate = useNavigate();

  useEffect(() => {
    document.title = "HMCTS DTS - Register";

    if (successState.state === "error") {
      document.title = `Error: ${document.title}`;
    }
  }, [successState.state]);

  /**
   * Handles the submission of the registration form.
   *
   * @remarks The function validates the input fields and sends a POST request to the server.
   * If the registration is successful, it redirects the user to the login page.
   * If there are errors, it updates the success state to display error messages.
   *
   * @param {React.FormEvent<HTMLFormElement>} e - The form submission event
   * @returns {Promise<void>} A promise that resolves when the submission process is complete
   */
  const handleSubmit = async (
    e: React.FormEvent<HTMLFormElement>
  ): Promise<void> => {
    e.preventDefault();

    setSuccessState({
      state: null,
      content: <></>,
    });

    const formData = new FormData(e.currentTarget);
    const name: RegisterData["name"] = formData.get("name") as string;
    const email: RegisterData["email"] = formData.get("email") as string;
    const password: RegisterData["password"] = formData.get(
      "new-password"
    ) as string;
    const confirmPassword: RegisterData["password"] = formData.get(
      "confirm-new-password"
    ) as string;

    if (password !== confirmPassword) {
      setSuccessState({
        state: "error",
        content: (
          <li>
            <a className="underline underline-offset-3" href="#new-password">
              Passwords do not match. Please re-enter your password.
            </a>
          </li>
        ),
      });
      return;
    }

    try {
      const response = await postRegisterUser({
        name,
        email,
        password,
      });

      if (response.status === 201) {
        navigate("/login");
      }
    } catch (error: unknown) {
      if (error instanceof AxiosError) {
        setSuccessState({
          state: "error",
          content: (
            <li>
              <a className="underline underline-offset-3" href="#sign-up-form">
                {error.response?.data?.message || "Registration failed"}
              </a>
            </li>
          ),
        });
      } else {
        setSuccessState({
          state: "error",
          content: (
            <li>
              <a className="underline underline-offset-3" href="#sign-up-form">
                {"An unexpected error occurred. Please try again."}
              </a>
            </li>
          ),
        });
      }
    }
  };

  return (
    <>
      <ErrorSummary visiblity={successState.state === "error"}>
        {successState.content}
      </ErrorSummary>
      <h1 className="m-[10px_0_30px_0] text-3xl md:text-5xl font-bold font-[Helvetica]">
        Create an account
      </h1>
      <p className="text-lg mb-[20px]">
        You'll need your password when you sign in online, so make it memorable.
      </p>
      <form
        id="sign-up-form"
        onSubmit={(e) => handleSubmit(e)}
        className="flex flex-col gap-4"
      >
        <div className="flex flex-col sm:w-1/2">
          <label
            className="text-lg md:text-2xl font-bold mb-[8px]"
            htmlFor="name"
          >
            Full name
          </label>
          <input
            className="focus:outline-3 focus:outline-[#ffdd00] border-2 focus:shadow-[inset_0_0_0_2px] border-black p-1"
            id="name"
            name="name"
            type="text"
            spellCheck="false"
            autoCapitalize="words"
            aria-description="Enter your email address"
            autoComplete="name"
          />
        </div>
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
