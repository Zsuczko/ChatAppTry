import { createContext} from "react"
import Login from "./elements/Login"
import { Toaster } from "./components/ui/sonner"


export type TokenContextType ={
  token: string,
  setToken: (token:string)=>void
}

export const tokenCtxt = createContext<TokenContextType>({token:"", setToken:()=>{}})



function App() {


  return (
    <>
    <tokenCtxt.Provider value={{token:"", setToken:()=>{}}}>
      <Login></Login>
      <Toaster position="top-center" richColors></Toaster>
    </tokenCtxt.Provider>
    </>
  )
}

export default App
