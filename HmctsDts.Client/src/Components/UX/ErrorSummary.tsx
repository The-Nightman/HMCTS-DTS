import { JSX } from "react";

interface ErrorSummaryProps {
  visiblity: boolean;
  children: React.ReactNode;
}

/**
 * Error summary box.
 *
 * @component
 * @param {object} props - The component props.
 * @param {boolean} props.visiblity - Controls whether the error summary is displayed.
 * @param {React.ReactNode} props.children - The content to display in the error summary, this should consist of list items.
 *
 * @returns {JSX.Element | null} The error summary element when visible, null otherwise.
 *
 * @example
 * ```tsx
 * <ErrorSummary visiblity={hasErrors}>
 *   <li><a href="#field1">Error message for field 1</a></li>
 *   <li><a href="#field2">Error message for field 2</a></li>
 * </ErrorSummary>
 * ```
 */
const ErrorSummary = ({
  visiblity,
  children,
}: ErrorSummaryProps): JSX.Element | null => {
  if (!visiblity) {
    return null;
  }

  return (
    <div className="border-5 border-[#d4351c] p-[20px] mt-[20px] mb-[30px]">
      <div role="alert">
        <h2 className="text-lg sm:text-2xl font-bold mb-[10px] sm:mb-[20px]">
          There is a problem
        </h2>
        <div className="sm:text-lg font-bold text-[#d4351c]">
          <ul className="mb-[5px]">{children}</ul>
        </div>
      </div>
    </div>
  );
};

export default ErrorSummary;
