import { useEffect, useState } from "react";
import "../../App.css";
import { useNavigate, useParams } from "react-router-dom";
import NormalMessage from "../UI components/NormalMessage";

function MainView() {
  const [navState, setNavState] = useState<string>("Main");
  const [errorMessage, setErrorMessage] = useState<string | null>(null);
  const [normalMessage, setNormalMessage] = useState<string | null>(null);
  const navigate = useNavigate();

  return (
    <>
      <div className="flex max-h-screen max-w-screen items-center justify-center">
        <div className="flex flex-col gap-5">
          <div className="flex flex-col max-md:w-90 md:w-xl bg-main-dark rounded-3xl p-6 gap-5">
            <span className="text-2xl font-bold self-start">{"all-attendances"}</span>

            <div className="flex self-center">{normalMessage && <NormalMessage text={normalMessage} />}</div>
          </div>
          <div className="flex flex-col max-md:w-90 md:w-xl bg-main-dark rounded-3xl p-6 gap-5">
            <span className="text-2xl font-bold self-start">{"statistics"}</span>
          </div>
        </div>
      </div>
    </>
  );
}

export default MainView;
