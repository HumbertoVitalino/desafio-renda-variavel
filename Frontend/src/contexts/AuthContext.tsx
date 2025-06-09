import { createContext, useState, useContext, useEffect, ReactNode } from 'react';

import api, { login as apiLogin } from '../services/api';
import { LoginUserRequest, UserLoginDto } from '../types/api';

interface IAuthContextData {
  user: UserLoginDto | null;
  loading: boolean;
  signIn(data: LoginUserRequest): Promise<void>;
  signOut(): void;
}

const AuthContext = createContext<IAuthContextData>({} as IAuthContextData);

interface IAuthProviderProps {
  children: ReactNode;
}

export const AuthProvider = ({ children }: IAuthProviderProps) => {
    const [user, setUser] = useState<UserLoginDto | null>(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        async function loadStoragedData() {
            const token = localStorage.getItem('authToken');
            const storedUser = localStorage.getItem('authUser');

            if (token && storedUser) {
                setUser(JSON.parse(storedUser));
                api.defaults.headers.common['Authorization'] = `Bearer ${token}`;
            }
            setLoading(false);
        }

        loadStoragedData();
    }, []);

    const signIn = async (data: LoginUserRequest) => {
        try {
            const response = await apiLogin(data);

            if (response.data.isValid && response.data.result) {
                const { token, ...userData } = response.data.result;

                localStorage.setItem('authToken', token);
                localStorage.setItem('authUser', JSON.stringify(userData));

                api.defaults.headers.common['Authorization'] = `Bearer ${token}`;
                
                setUser(userData as UserLoginDto);
            } else {
                throw new Error(response.data.errorMessages?.join(', ') || 'Dados inválidos.');
            }
        } catch (error) {
            throw error;
        }
    };

    const signOut = () => {
        localStorage.removeItem('authToken');
        localStorage.removeItem('authUser');
        delete api.defaults.headers.common['Authorization'];
        setUser(null);
    };
    
    return (
        <AuthContext.Provider value={{ user, loading, signIn, signOut }}>
            {children}
        </AuthContext.Provider>
    );
};

export function useAuth(): IAuthContextData {
    const context = useContext(AuthContext);
    if (!context) {
        throw new Error('useAuth must be used within an AuthProvider.');
    }
    return context;
}