import "./App.css";
import MainView from "../src/Layout/Views/MainView";
import EditView from "../src/Layout/Views/EditView";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/:id?" element={<MainView />} />
        <Route path="/Edit/:id?" element={<EditView />} />
      </Routes>
    </Router>
  );
}

export default App;
