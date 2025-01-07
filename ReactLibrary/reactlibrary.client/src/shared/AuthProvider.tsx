import { useState, createContext, useContext, PropsWithChildren } from 'react';


type AuthContext = {
    isadmin?: bool | null
    username?: string | null
    handleLogin: (data) => Promise<number>;
    handleLogout: () => Promise<void>;
};

const authContext = createContext<AuthContext | undefined>(undefined)

type AuthProviderProps = PropsWithChildren

export function AuthProvider({ children }: AuthProviderProps) {
    const [isadmin, setIsadmin] = useState<bool | undefined>();
    const [username, setUsername] = useState<string | undefined>();


    async function handleLogin(data) {
        console.log("\n********\n\n in send request \n\n ******\n");
        console.log(data)
        const requestOptions = {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(data)
        };
        const response = await fetch('/user/login', requestOptions);
        if (response.ok) {
            const data = await response.json();
            console.log(data)
            setIsadmin(data.isadmin)
            setUsername(data.username)
            return 200;
        }
        else if (response.status == 406) {
            setIsadmin(undefined)
            setUsername(undefined)
            return 406;
        }
        else {
            setIsadmin(undefined)
            setUsername(undefined)
            return 400;
        }
    }

    async function handleLogout() {
        console.log("in handle logout")
        const response = await fetch('/user/logout');
        if (response.ok) {
            console.log("logout")
            setIsadmin(null)
            setUsername(null)
            return 200;
        }
    }

    return <authContext.Provider
        value={{
            isadmin,
            username,
            handleLogin,
            handleLogout,
        }}>
        {children}
    </authContext.Provider>
}

export function useAuth() {
    const context = useContext(authContext);

    if (context === undefined) {
        throw new Error("Use auth must be used inside of auth provider");
    }

    return context;
}