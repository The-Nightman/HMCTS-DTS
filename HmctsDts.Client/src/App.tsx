import { Navigate, Route, Routes } from "react-router";
import { UnauthenticatedLayout } from "./Layouts";
import { RegisterPage, StartPage } from "./Pages";

const App = () => {
  return (
    <>
      <Routes>
        <Route path="/" element={<UnauthenticatedLayout />}>
          <Route
            index
            // We can't set /start as an entry point but we can redirect
            element={<Navigate to={"/start"} />}
          />
          <Route path="/start" element={<StartPage />} />
          <Route path="/register" element={<RegisterPage />} />
          <Route path="/login" element={<p>login</p>} />
          <Route path="*" element={<Navigate to={"/start"} />} />
        </Route>
        <Route path="*" element={<div>404 Not Found</div>} />
      </Routes>
    </>
  );
};

export default App;
