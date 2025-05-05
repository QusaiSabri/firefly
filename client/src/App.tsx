import { ThemeProvider } from '@/components/theme-provider';
import { AppSidebar } from '@/components/app-sidebar';
import { SiteHeader } from '@/components/site-header';
import { SidebarInset, SidebarProvider } from '@/components/ui/sidebar';
import Home from '@/Views/Home';
import { Routes, Route } from 'react-router';
import Library from '@/Views/Library';

export default function Page() {
  return (
    <ThemeProvider defaultTheme='dark' storageKey='vite-ui-theme'>
      <div className='[--header-height:calc(theme(spacing.14))]'>
        <SidebarProvider className='flex flex-col'>
          <div className='flex flex-1 overflow-x-hidden'>
            <AppSidebar />
            <SidebarInset>
              <div className='flex flex-1 flex-col gap-0 p-0'>
                <SiteHeader />
                <Routes>
                  <Route path='/' element={<Home />} />
                  <Route path='/library' element={<Library />} />
                </Routes>
              </div>
            </SidebarInset>
          </div>
        </SidebarProvider>
      </div>
    </ThemeProvider>
  );
}
