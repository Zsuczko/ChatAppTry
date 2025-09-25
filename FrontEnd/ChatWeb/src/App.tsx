import { createContext, useEffect, useState} from "react"
import Login from "./elements/Login"
import { Toaster } from "./components/ui/sonner"
import { Route, Routes, useNavigate } from "react-router-dom"
import Home from "./elements/Home"


export type TokenContextType ={
  token: string,
  setToken: (token:string)=>void
}

export const tokenCtxt = createContext<TokenContextType>({token:"", setToken:()=>{}})



function App() {

  const navigate = useNavigate()

  const [token, setToken] = useState<string>("")


  useEffect(()=>{
    const storedToken = localStorage.getItem("token");
    if(storedToken){
      setToken(storedToken)
    }
    else{
      navigate("/login")
    }
    // console.log(token," - " ,storedToken)
  },[])


  return (
    <>
    <tokenCtxt.Provider value={{token:token, setToken:setToken}}>

        <Routes>
          <Route path="/" element={<Home></Home>}></Route>
          <Route path="/login" element={<Login></Login>}></Route>
        </Routes>
        <Toaster position="top-center" richColors></Toaster>
    </tokenCtxt.Provider>
    </>
  )
}

export default App
