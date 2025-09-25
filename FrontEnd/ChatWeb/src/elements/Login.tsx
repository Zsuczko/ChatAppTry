import { tokenCtxt } from '@/App'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { LoginRequest } from '@/functions/ApiRequests'
import { useContext, useState } from 'react'
import { toast } from 'sonner'

const Login = () => {

    const [username, setUsername] = useState<string>("")
    const [password, setPassword] = useState<string>("")
    const token = useContext(tokenCtxt)



    const LoginToApi = async () => {

        console.log({username, password})
        try{
            const data = await LoginRequest({username, password})
            console.log(data.token)
            token.setToken(data.token);
        }
        catch (error){
            if(error instanceof Error){
                toast.error(error.message)
            }
        }

    }

    return (
        <main className='flex items-center justify-center min-h-screen'>
            <div className='border-2 border-black p-10 rounded-2xl flex flex-col gap-6 w-[20%]'>
                <h1 className='text-2xl font-bold'>Login</h1>
                <Input placeholder='username' value={username} onChange={(e)=>{setUsername(e.target.value)}}></Input>
                <Input type='password' placeholder='password' value={password} onChange={(e)=>{setPassword(e.target.value)}}></Input>
                <Button onClick={LoginToApi}>Login</Button>
            </div>
        </main>
    )
}

export default Login