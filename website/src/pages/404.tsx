import type {ReactNode} from 'react';
import Link from '@docusaurus/Link';
import Layout from '@theme/Layout';
import Heading from '@theme/Heading';

export default function NotFound(): ReactNode {
  return (
    <Layout title="Page Not Found">
      <main className="container margin-vert--xl">
        <div className="row">
          <div className="col col--6 col--offset-3">
            <Heading as="h1" className="hero__title">
              Page Not Found
            </Heading>
            <p>We could not find what you were looking for.</p>
            <p>
              The page you requested may have been moved or no longer exists.
            </p>
            <Link
              className="button button--primary button--lg"
              to="/docs/getting-started/installation">
              Go to Documentation
            </Link>
          </div>
        </div>
      </main>
    </Layout>
  );
}
