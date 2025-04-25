import { JSX, useEffect, useState } from "react";
import { Link } from "react-router";
import { AxiosError } from "axios";
import ErrorSummary from "../Components/UX/ErrorSummary";
import { postLoginUser } from "../api/postLoginUser";
import { LoginData } from "../types/loginData";

interface SuccessState {
  state: "success" | "error" | null;
  content: React.ReactNode;
}

/**
 * LoginPage Component
 *
 * A component that renders a user login form with email and password fields.
 * The form includes validation for:
 * - Email field with proper email format
 * - Password field with specific requirements:
 *   - At least 8 characters
 *   - At least 1 capital letter
 *   - At least 1 lowercase letter
 *   - At least 1 number
 *   - At least 1 special character
 *
 * @remarks
 * Uses GOV.UK-style design patterns for styling and accessibility.
 *
 * @returns {JSX.Element} A login form with email and password fields
 */
const LoginPage = (): JSX.Element => {
  const [successState, setSuccessState] = useState<SuccessState>({
    state: null,
    content: <></>,
  });
  // const navigate = useNavigate();

  useEffect(() => {
    document.title = "HMCTS DTS - Sign In";

    if (successState.state === "error") {
      document.title = `Error: ${document.title}`;
    }
  }, [successState.state]);

  /**
   * Handles the submission of the login form.
   *
   * !not yet fully implemented
   *
   * @remarks The function validates the input fields and sends a POST request to the server.
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
    const email: LoginData["email"] = formData.get("email") as string;
    const password: LoginData["password"] = formData.get("password") as string;

    try {
      const response = await postLoginUser({
        email,
        password,
      });

      // not yet implemented
      console.log(response.data);
      alert(`Login successful! Welcome back, ${response.data.name}.`);
    } catch (error: unknown) {
      if (error instanceof AxiosError) {
        setSuccessState({
          state: "error",
          content: (
            <li>
              <a className="underline underline-offset-3" href="#sign-up-form">
                {error.response?.data?.message || "Sign in failed"}
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
        Sign in to your HMCTS DTS Task Manager account
      </h1>
      <form
        id="sign-in-form"
        onSubmit={(e) => handleSubmit(e)}
        className="flex flex-col gap-4"
      >
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
          <label className="text-lg md:text-2xl font-bold" htmlFor="password">
            Password
          </label>
          <input
            className="focus:outline-3 focus:outline-[#ffdd00] border-2 focus:shadow-[inset_0_0_0_2px] border-black p-1"
            id="password"
            name="password"
            type="password"
            aria-description="Enter your password"
            autoComplete="current-password"
            pattern="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$"
          />
        </div>
        <button
          className="w-fit text-center justify-center bg-[#00823b] hover:bg-[#1f6b43] shadow-[0_2px_0_#003418] text-white py-2 px-4 mb-[30px] border-[2px] border-transparent active:border-[#ffdd00] not-active:focus:bg-[#ffdd00] not-active:focus:text-black select-none"
          type="submit"
          draggable="false"
          aria-label="Sign in"
        >
          Sign in
        </button>
      </form>
      <div>
        <h2 className="text-lg md:text-2xl font-bold mb-[20px]">
          If you haven't made an account yet
        </h2>
        <p className="mb-[20px]">
          <Link
            className="underline underline-offset-3 text-[#1d70b8] hover:text-[#003078] visited:text-[#4c2c92]"
            to="/login"
            aria-label="Create an account"
          >
            Go here to create an account
          </Link>{" "}
          to be able to use the service.
        </p>
      </div>
    </>
  );
};

export default LoginPage;
