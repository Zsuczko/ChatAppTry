

export interface LoginPostBody{
    username: string, 
    password: string
}

export interface LoginResponse {
    token:string
}


export async function LoginRequest(body: LoginPostBody): Promise<LoginResponse> {
    const response = await fetch("https://localhost:7125/api/auth/login",{
        method: "POST",
        headers: {
            "Content-Type": "application/json",
          },
        body: JSON.stringify(body)
    });

    if(response.status === 401){
        throw new Error("Wrong password or username")
    }
    if(!response.ok){
        throw new Error("Something went wrong")
    }


    return await response.json()
}