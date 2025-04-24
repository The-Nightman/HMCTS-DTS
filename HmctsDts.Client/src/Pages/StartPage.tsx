import { JSX, useEffect } from "react";
import { Link } from "react-router";

/**
 * StartPage Component
 *
 * This component serves as the landing page for the HMCTS Task Manager service.
 * It contains:
 * - A heading with the service name
 * - A list of features the service provides
 * - Requirements for using the service
 * - A call-to-action button to create an account
 * - A section for users who already have an account with a login link
 *
 * Navigation:
 * - "Create an account" button links to /register
 * - "Sign in" link directs to /login
 *
 * @returns {JSX.Element} The rendered StartPage component
 */
const StartPage = (): JSX.Element => {
  useEffect(() => {
    document.title = "HMCTS DTS - Start";
  }, []);

  return (
    <>
      <div className="flex flex-col md:gap-8">
        <div>
          <h1 className="m-[10px_0_30px_0] text-3xl md:text-5xl font-bold font-[Helvetica]">
            HMCTS Task Manager
          </h1>
          <p className="mb-[20px]">Use this service to:</p>
          <ul className="mb-[20px] list-disc list-inside">
            <li>Manage your tasks</li>
            <li>Track your progress</li>
            <li>Collaborate with your team</li>
          </ul>
          <p className="mb-[20px]">
            To use this service you must have an email address, an ID will be
            assigned for you.
          </p>
          <Link
            className="max-sm:flex text-center justify-center bg-[#00823b] hover:bg-[#1f6b43] shadow-[0_2px_0_#003418] text-white py-2 px-4 mb-[30px] border-[2px] border-transparent active:border-[#ffdd00] not-active:focus:bg-[#ffdd00] not-active:focus:text-black select-none"
            to="/register"
            draggable="false"
            aria-label="Create an account"
          >
            Create an account
          </Link>
        </div>
        <div>
          <h2 className="text-lg md:text-2xl font-bold mb-[20px]">
            If you already have an account
          </h2>
          <p className="mb-[20px]">
            <Link
              className="underline underline-offset-3 text-[#1d70b8] hover:text-[#003078] visited:text-[#4c2c92]"
              to="/login"
              aria-label="Login to your account"
            >
              Sign in to your HMCTS DTS Task Manager account
            </Link>{" "}
            if you already have one.
          </p>
        </div>
      </div>
    </>
  );
};

export default StartPage;
