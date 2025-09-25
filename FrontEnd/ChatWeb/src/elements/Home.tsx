import { tokenCtxt } from "@/App";
import { useContext, useEffect } from "react"
import { useNavigate } from "react-router-dom"

const Home = () => {


    const {token} = useContext(tokenCtxt)


    return (
        <main>
            <h1 className="font-bold text-2xl m-10">Hello</h1>
        </main>
    )
}

export default Home