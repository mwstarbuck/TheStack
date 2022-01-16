import logo from './logo.svg';
import './App.css';
import { Home } from './Home';
import Department from './Department';
import Employee from './Employee';
import { BrowserRouter, Routes, Route, Link } from 'react-router-dom';
/* 
1.  The <h3 className bellow is boostrap. copied css link in the head
  section of index.html
2. Copy bootstrap Bundle <script> reference and paste before the closing </body> tag in inedx.html
  to enable bootstrap components
3. npm install 'react-router-dom' to enable routing in the app
4. Import BrwserRouter, Route, Swithc and NavLink from 'react-router-dom'
5. Add the Navigation Menu  <nav> -- Unordered List with Navlinks
6. Add the <Routes> tag and <Route> tags to define the route paths there
7. Surround the parent div in the render with <BrowserRouter>. tags 
    OR surround the entire app <App /> with BrowseRouter tags in the index.js file 
8. Create a file.js that stores all API endpoints and import into components that will use a the API
*/
function App() {
  return (
    <BrowserRouter>
      <div className="App container">
        <h3 className='d-flex justify-content-center m-3'>
          React JS Frontend
        </h3>

        <nav className='navbar navbar-expand-sm bg-light navbar-dark'>
          <ul className='navbar-nav'>
            <li className='nav-item m-1'>
              <Link className="btn btn-light btn-outline-primary" to="/home">
                Home
              </Link>
            </li>
            <li className='nav-item m-1'>
              <Link className="btn btn-light btn-outline-primary" to="/department">
                Department
              </Link>
            </li>          <li className='nav-item m-1'>
              <Link className="btn btn-light btn-outline-primary" to="/employee">
                Employee
              </Link>
            </li>
          </ul>
        </nav>

        <Routes>
          <Route path='/home' element={<Home />} />
          <Route path='/department' element={<Department />} />
          <Route path='/employee' element={<Employee />} />
        </Routes>
      </div>
    </BrowserRouter>
  );
}

export default App;
