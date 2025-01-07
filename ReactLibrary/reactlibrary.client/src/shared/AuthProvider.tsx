import { useState, createContext, useContext, PropsWithChildren } from 'react';


type AuthContext = {
    role?: string | null
    username?: string | null
    handleLogin: (data) => Promise<number>;
    handleLogout: () => Promise<void>;
};

const authContext = createContext<AuthContext | undefined>(undefined)

type AuthProviderProps = PropsWithChildren

export function AuthProvider({ children }: AuthProviderProps) {
    const [role, setRole] = useState<string | undefined>();
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
            setRole(data.role)
            setUsername(data.username)
            return 200;
        }
        else if (response.status == 406) {
            setRole(undefined)
            setUsername(undefined)
            return 406;
        }
        else {
            setRole(undefined)
            setUsername(undefined)
            return 400;
        }
    }

    async function handleLogout() {
        //make call to user/logout
    }

    return <authContext.Provider
        value={{
            role,
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