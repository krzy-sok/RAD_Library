import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
//import './index.css'
import "./css/bootstrap.min.css"
import "./css/site.css"
import App from './App.tsx'
import { Catalogue } from "./books/Catalogue.tsx"
import { BrowserRouter, Route, Routes } from 'react-router-dom'
import { BookDetails, BookDelete } from "./books/BookDetails.tsx"
import { BookEdit, BookCreate } from "./books/BookEdit.tsx"
import { LoginForm } from "./LoginComponent.tsx"
import { RegisterForm } from "./RegisterComponent.tsx"
import { AuthProvider } from "./shared/AuthProvider.tsx"
import { UsersList } from "./users/UsersList.tsx"
import { LeaseList, LeaseListInactive } from "./leases/LeasesList.tsx"
import { UserDetails } from "./users/UserDetails.tsx"
import { LeaseDetails } from "./leases/LeaseDetails.tsx"
import { UserLeases } from "./users/UserLeases.tsx"
import { LeaseEdit } from "./leases/LeaseEdit.tsx"
import { DeleteUser } from "./users/UserDelete.tsx"

createRoot(document.getElementById('root')!).render(
    <StrictMode>
        <AuthProvider>
            <BrowserRouter>
                <Routes>
                    <Route path='/' element={<App />} />
                    <Route path='/catalogue' element={<Catalogue />} />
                    <Route path='/detailsBook/:bookId' element={<BookDetails />} />
                    <Route path='/editBook/:bookId' element={<BookEdit />} />
                    <Route path='/deleteBook/:bookId' element={<BookDelete />} />
                    <Route path='/createBook' element={<BookCreate />} />
                    <Route path='/login' element={<LoginForm />} />
                    <Route path='/registration' element={<RegisterForm />} />
                    <Route path='/library-users' element={<UsersList />} />
                    <Route path='/book-leases' element={<LeaseList />} />
                    <Route path='/book-leases/inactive' element={<LeaseListInactive />} />
                    <Route path='/user' element={<UserDetails />} />
                    <Route path='/user/delete' element={<DeleteUser />} />
                    <Route path='/user-leases' element={<UserLeases />} />
                    <Route path='/leaseDetails/:leaseId' element={<LeaseDetails />} />
                    <Route path='/editLease/:leaseId' element={<LeaseEdit />} />
                </Routes>
            </BrowserRouter>
        </AuthProvider>
  </StrictMode>,
)
